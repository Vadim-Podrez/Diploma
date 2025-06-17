import json, math, random, time
import paho.mqtt.publish as publish


BROKER = "localhost"  # або ip твоєї машини, якщо брокер не локально
PORT = 1883
TOPIC = "rf/events/test"
EVENTS = 50

sensors = [
    # id, type,           lat,        lng,      radius
    ( 1, 'radar', 50.460, 30.520, 1200),
    ( 2, 'audio', 50.480, 30.500, 2000),
    ( 3, 'radar', 50.440, 30.550, 2000),
    ( 4, 'radar', 50.500, 30.600, 1300),
    ( 5, 'audio', 50.470, 30.540, 1000),
    ( 6, 'radar', 50.540, 30.510, 2500),
    ( 8, 'audio', 50.560, 30.450, 1500),
    ( 9, 'radar', 50.520, 30.660, 2200),
    (10, 'radar', 50.530, 30.700, 2800),
    (12, 'audio', 50.490, 30.380, 1200),
    (13, 'radar', 50.390, 30.380, 2500),
    (14, 'radar', 50.410, 30.420, 2000),
    (15, 'audio', 50.350, 30.500, 1800),
    (16, 'radar', 50.330, 30.580,  850),
    (17, 'radar', 50.380, 30.690, 2300),
    (18, 'radar', 50.400, 30.740, 2600),
    (19, 'audio', 50.600, 30.600, 1600),
    (20, 'radar', 50.300, 30.350, 2400),
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
    time.sleep(0.2)

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
