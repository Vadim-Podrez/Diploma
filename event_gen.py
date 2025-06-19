import json, math, random, time
import paho.mqtt.publish as publish


BROKER = "localhost"  # або ip твоєї машини, якщо брокер не локально
PORT = 1883
TOPIC = "rf/events/test"
EVENTS = 5

sensors = [
    # ─── радарна корона (~650 м від HUB) ───
    ( 1, 'radar',  50.54153, 30.59545, 1000),  # W
    ( 2, 'radar',  50.54443, 30.61172, 1000),  # NE
    ( 3, 'radar',  50.53893, 30.61302, 1000),  # SE
    ( 4, 'radar',  50.54423, 30.59902, 1000),  # NW

    # ─── акустичний пояс (~200 м) ───
    ( 5, 'audio',  50.54343, 30.60462,  400),  # N
    ( 6, 'audio',  50.53993, 30.60702,  400),  # SE
    ( 7, 'audio',  50.53993, 30.60222,  400),  # SW

    # ─── оптичні камери (як video-сенсори) ───
    ( 8, 'video',  50.53943, 30.59922,  450),  # cam-1
    ( 9, 'video',  50.54393, 30.60922,  450),  # cam-2
    (10, 'video',  50.54143, 30.61052,  450)   # cam-3
]

def random_point(lat0, lng0, radius_m):
    r   = radius_m * math.sqrt(random.random())
    phi = random.uniform(0, 2*math.pi)
    dlat = (r * math.cos(phi)) / 111_000
    dlng = (r * math.sin(phi)) / (111_000 * math.cos(math.radians(lat0)))
    return lat0 + dlat, lng0 + dlng

for _ in range(EVENTS):
    sid, stype, slat, slng, sradius = random.choice(sensors)
    lat, lng = random_point(slat, slng, sradius)

    msg = {
        "sensorId": sid,
        "coords": { "X": round(lat,6), "Y": round(lng,6) },
        "payload": {
            "type":   stype,                 # radar / audio / video
            "signal": random.randint(50,100),
            "alarm":  random.random() < 0.15
        }
    }
    publish.single(TOPIC, json.dumps(msg), hostname=BROKER, port=PORT)
    print("Sent", sid, msg["payload"]["type"], "-", (lat,lng))
    time.sleep(1)

# for i in range(5):  # 10 подій, або зроби while True для нескінченних
#     msg = {
#         "sensorId": random.randint(1, 5),
#         "coords": random_coords(),
#         "payload": {
#             "type": random.choice(["rf", "audio", "video"]),
#             "signal": random.randint(50, 100),
#             "alarm": random.choice([True])
#         }
#     }
