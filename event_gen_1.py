import json, math, time
import random                      # ### FIX ###
import paho.mqtt.publish as publish

# ─────────── MQTT ───────────
BROKER = "localhost"
PORT   = 1883
TOPIC  = "rf/events/test"

# ─────────── СЕНСОРИ (Дубровка) ───────────
SENSORS = [
    # id, тип,            lat,        lng,     R (м)
    ( 1, 'radar',  50.54153, 30.59545, 1000),  # W
    ( 2, 'radar',  50.54443, 30.61172, 1000),  # NE
    ( 3, 'radar',  50.53893, 30.61302, 1000),  # SE
    ( 4, 'radar',  50.54423, 30.59902, 1000),  # NW
    ( 5, 'audio',  50.54343, 30.60462,  400),  # N
    ( 6, 'audio',  50.53993, 30.60702,  400),  # SE
    ( 7, 'audio',  50.53993, 30.60222,  400),  # SW
    ( 8, 'video',  50.53943, 30.59922,  450),  # cam-1
    ( 9, 'video',  50.54393, 30.60922,  450),  # cam-2
    (10, 'video',  50.54143, 30.61052,  450)   # cam-3
]

# ─────────── ПАРАМЕТРИ СЦЕНАРІЮ ───────────
START_LAT, START_LNG   = 50.54550, 30.58500     # точка появи
TARGET_LAT, TARGET_LNG = 50.54153, 30.60462     # ціль
STEPS        = 30
PAUSE_SEC    = 0.30
ALARM_CHANCE = 0.25

# ─────────── ДОПОМІЖНІ Ф-ЦІЇ ───────────
def haversine_m(lat1, lon1, lat2, lon2):
    R = 6371000
    phi1, phi2 = math.radians(lat1), math.radians(lat2)
    dphi  = math.radians(lat2 - lat1)
    dlamb = math.radians(lon2 - lon1)
    a = math.sin(dphi/2)**2 + math.cos(phi1)*math.cos(phi2)*math.sin(dlamb/2)**2
    return 2*R*math.asin(math.sqrt(a))

def lerp(a, b, t):
    return a + (b - a) * t

# ─────────── ІМІТАЦІЯ РУХУ ДРОНА ───────────
for step in range(STEPS + 1):
    t   = step / STEPS
    lat = lerp(START_LAT,  TARGET_LAT,  t)
    lng = lerp(START_LNG,  TARGET_LNG,  t)

    for sid, stype, slat, slng, sr in SENSORS:
        if haversine_m(lat, lng, slat, slng) <= sr:
            msg = {
                "sensorId": sid,
                "coords": { "X": round(lat,6), "Y": round(lng,6) },
                "payload": {
                    "type":   stype,
                    "signal": 80,
                    "alarm":  (random.random() < ALARM_CHANCE)  # ### FIX ###
                }
            }
            publish.single(TOPIC, json.dumps(msg), hostname=BROKER, port=PORT)

    time.sleep(PAUSE_SEC)
