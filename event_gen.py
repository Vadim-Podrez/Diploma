import time
import random
import json
import paho.mqtt.publish as publish

BROKER = "localhost"  # або ip твоєї машини, якщо брокер не локально
PORT = 1883
TOPIC = "rf/events/test"

def random_coords():
    lat = round(random.uniform(50.34, 50.52), 6)
    lng = round(random.uniform(30.40, 30.66), 6)
    return [lat, lng]

for i in range(5):  # 10 подій, або зроби while True для нескінченних
    msg = {
        "sensorId": f"test-{random.randint(1,5)}",
        "coords": random_coords(),
        "payload": {
            "type": random.choice(["rf", "audio", "video"]),
            "signal": random.randint(50, 100),
            "alarm": random.choice([True, False])
        }
    }
    publish.single(TOPIC, payload=json.dumps(msg), hostname=BROKER, port=PORT)
    print("Sent:", msg)
    time.sleep(2)