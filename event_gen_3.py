#!/usr/bin/env python
# event_gen_3.py  –  три дрони, послідовні події radar+audio+video
import json, math, random, time
import paho.mqtt.publish as publish

BROKER       = "localhost"
PORT         = 1883
TOPIC        = "rf/events/test"
STEPS        = 25          # точок на маршрут
SLEEP_SEC    = 0.30        # пауза між точками
ALARM_CHANCE = 0.15        # імовірність alarm=true

# ── сенсори з «польової» схеми ──────────────────────────────────────────────
SENSORS = [
    ( 1, 'radar', 50.54153, 30.59545, 1000),
    ( 2, 'radar', 50.54443, 30.61172, 1000),
    ( 3, 'radar', 50.53893, 30.61302, 1000),
    ( 4, 'radar', 50.54423, 30.59902, 1000),
    ( 5, 'audio', 50.54343, 30.60462,  400),
    ( 6, 'audio', 50.53993, 30.60702,  400),
    ( 7, 'audio', 50.53993, 30.60222,  400),
    ( 8, 'video', 50.53943, 30.59922,  450),
    ( 9, 'video', 50.54393, 30.60922,  450),
    (10, 'video', 50.54143, 30.61052,  450),
]

# ── ТРИ маршрути дронів ────────────────────────────────────────────────────
DRONES = [
    dict(name="drone-A",     # Північ → центр
         start=(50.54550, 30.58900),
         end  =(50.54050, 30.61500)),

    dict(name="drone-B",     # Південь-захід → центр
         start=(50.53700, 30.59000),
         end  =(50.54400, 30.60000)),

    dict(name="drone-C",     # Східне поле → центр
         start=(50.54300, 30.62000),
         end  =(50.54000, 30.60000)),
]

# ── допоміжні функції ──────────────────────────────────────────────────────
def lerp(a, b, t):              # інтерполяція
    return a + (b - a) * t

def path_positions(start, end, steps):
    for i in range(steps):
        t = i / (steps - 1)
        yield (lerp(start[0], end[0], t),
               lerp(start[1], end[1], t))

def haversine_m(lat1, lon1, lat2, lon2):
    R = 6371000
    φ1, φ2 = math.radians(lat1), math.radians(lat2)
    dφ  = math.radians(lat2 - lat1)
    dλ  = math.radians(lon2 - lon1)
    a = math.sin(dφ/2)**2 + math.cos(φ1)*math.cos(φ2)*math.sin(dλ/2)**2
    return 2*R*math.asin(math.sqrt(a))

def closest_sensor(lat, lng, stype):
    best, dmin = None, 1e9
    for sid, tp, slat, slng, _ in SENSORS:
        if tp != stype:
            continue
        d = haversine_m(lat, lng, slat, slng)
        if d < dmin:
            dmin, best = d, sid
    return best

# ── основний цикл ──────────────────────────────────────────────────────────
step = 0
for drone in DRONES:
    for lat, lng in path_positions(drone["start"], drone["end"], STEPS):
        step += 1

        sid_r = closest_sensor(lat, lng, 'radar')
        sid_a = closest_sensor(lat, lng, 'audio')
        sid_v = closest_sensor(lat, lng, 'video')

        for sid, tp in [(sid_r, 'radar'), (sid_a, 'audio'), (sid_v, 'video')]:
            if sid is None:
                continue
            msg = {
                "sensorId": sid,
                "coords": { "X": round(lat, 6), "Y": round(lng, 6) },
                "payload": {
                    "type":   tp,
                    "signal": random.randint(60, 100),
                    "alarm":  random.random() < ALARM_CHANCE
                }
            }
            publish.single(TOPIC, json.dumps(msg),
                           hostname=BROKER, port=PORT)
            print(f"[{step:03d}] {tp:<5} #{sid:02d}  ({lat:.6f}, {lng:.6f})")

        time.sleep(SLEEP_SEC)
