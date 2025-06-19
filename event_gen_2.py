#!/usr/bin/env python
# event_gen_2.py   –   два дрони, послідовні події radar+audio+video
import json, math, random, time
import paho.mqtt.publish as publish

BROKER      = "localhost"         # IP брокера
PORT        = 1883
TOPIC       = "rf/events/test"
STEPS       = 25                  # скільки "кадрів" на маршрут
SLEEP_SEC   = 0.3                 # пауза між подіями
ALARM_CHANCE = 0.15               # імовірність alarm=true

# ───── нові координати сенсорів ─────
SENSORS = [
    # id,  type,          lat,       lng,      radius
    ( 1, 'radar', 50.54153, 30.59545, 1000),
    ( 2, 'radar', 50.54443, 30.61172, 1000),
    ( 3, 'radar', 50.53893, 30.61302, 1000),
    ( 4, 'radar', 50.54423, 30.59902, 1000),
    ( 5, 'audio', 50.54343, 30.60462,  400),
    ( 6, 'audio', 50.53993, 30.60702,  400),
    ( 7, 'audio', 50.53993, 30.60222,  400),
    ( 8, 'video', 50.53943, 30.59922,  450),   # камери зручніше
    ( 9, 'video', 50.54393, 30.60922,  450),
    (10, 'video', 50.54143, 30.61052,  450),
]

# ───── два дрони: старт → фініш ─────
DRONES = [
    dict(name="drone-A",
         start=(50.54550, 30.58900),
         end  =(50.54050, 30.61500)),
    dict(name="drone-B",
         start=(50.53700, 30.59000),
         end  =(50.54400, 30.60000)),
]

def lerp(a, b, t):          # лінійна інтерполяція координат
    return a + (b - a) * t

def path_positions(start, end, steps):
    """Генерує точки шляху A→B (вкл. кінцеву)."""
    for i in range(steps):
        t = i / (steps - 1)
        yield (lerp(start[0], end[0], t),
               lerp(start[1], end[1], t))

def closest_sensor(lat, lng, stype):
    """Шукає найближчий сенсор заданого type."""
    best = None
    dmin = 1e9
    for sid, tp, slat, slng, _ in SENSORS:
        if tp != stype: 
            continue
        d = haversine_m(lat, lng, slat, slng)
        if d < dmin:
            dmin, best = d, sid
    return best

def haversine_m(lat1, lon1, lat2, lon2):
    """Відстань між точками на поверхні Землі, метри."""
    R = 6371000
    phi1, phi2 = math.radians(lat1), math.radians(lat2)
    dphi  = math.radians(lat2 - lat1)
    dlamb = math.radians(lon2 - lon1)
    a = math.sin(dphi/2)**2 + math.cos(phi1)*math.cos(phi2)*math.sin(dlamb/2)**2
    return 2*R*math.asin(math.sqrt(a))

step = 0
for drone in DRONES:
    for lat, lng in path_positions(drone["start"], drone["end"], STEPS):
        step += 1

        # 1) radar
        sid_r = closest_sensor(lat, lng, 'radar')
        # 2) audio
        sid_a = closest_sensor(lat, lng, 'audio')
        # 3) video
        sid_v = closest_sensor(lat, lng, 'video')

        for sid, tp in [(sid_r,'radar'), (sid_a,'audio'), (sid_v,'video')]:
            if sid is None:          # буває, якщо type відсутній
                continue
            msg = {
                "sensorId": sid,
                "coords": { "X": round(lat,6), "Y": round(lng,6) },
                "payload": {
                    "type":   tp,
                    "signal": random.randint(60, 100),
                    "alarm":  random.random() < ALARM_CHANCE
                }
            }
            publish.single(TOPIC, json.dumps(msg), hostname=BROKER, port=PORT)

        time.sleep(SLEEP_SEC)
