#!/usr/bin/env python3

import json
import logging
import os

from filelock import FileLock
from flask import Flask, request
import jsonschema

# Where should hiscores be stored?
HISCORE_FILE = "a@_/hiscore.json"
# Make sure hiscore file is no larger than this. Entries with worst scores will
# be dropped when this is exceeded.
HISCORE_SIZE_LIMIT = 2**24
# Reject recordings that have 1MB or larger
RECORD_SIZE_LIMIT_B = 2**20

HISCORE_JSON_SCHEMA = {
    "type": "object",
    "properties": {
        "Track": { "type": "integer", "minimum": 0, "maximum": 1 },
        "Nickname": { "type": "string" },
        "Result": { "type": "integer", "minimum": 0, "maximum": 1 },
        "TimeElapsed": { "type": "number" },
        "StartPosition": {
            "type": "object",
            "properties": {
                "x": { "type": "number" },
                "y": { "type": "number" },
                "z": { "type": "number" }
            }
        },
        "StartRotation": {
            "type": "object",
            "properties": {
                "x": { "type": "number" },
                "y": { "type": "number" },
                "z": { "type": "number" },
                "w": { "type": "number" }
            }
        },
        "Settings": {
            "type": "object",
            "properties": {
                "Speed": { "type": "number" },
                "RotateSpeed": { "type": "number" }
            }
        },
        "Frames": {
            "type": "array",
            "items": {
                "type": "object",
                "properties": {
                    "Frame": { "type": "integer" },
                    "Left": { "type": "number" },
                    "Right": { "type": "number" },
                    "Velocity": { "type": "number" }
                }
            }
        }
    }
}

app = Flask(__name__)


def hiscore_goodness(hiscore):
    if hiscore["Result"] == 0:
        # make sure all "finished" entries are larger than "unfinished"
        # also, lower time == better
        return (1, -hiscore["TimeElapsed"])
    else:
        return (0, hiscore["TimeElapsed"])


def hiscore_summary(hiscore):
    return "{nick}: {_not}finished, {time}".format(
            nick=hiscore["Nickname"],
            _not=("not" if hiscore["Result"] else ""),
            time=hiscore["TimeElapsed"])


def partition(pred, iterable):
    'Use a predicate to partition entries into false entries and true entries'
    from itertools import tee, filterfalse
    # partition(is_odd, range(10)) --> 0 2 4 6 8   and  1 3 5 7 9
    t1, t2 = tee(iterable)
    return list(filterfalse(pred, t1)), list(filter(pred, t2))


@app.route("/hiscores", methods=['GET', 'POST'])
def hiscores():
    if request.method == 'GET':
        with open(HISCORE_FILE) as f:
            return f.read()
    elif request.method == 'POST':
        record = request.get_data(RECORD_SIZE_LIMIT_B, as_text=True)
        if len(record) >= RECORD_SIZE_LIMIT_B:
            return 'Save file too large', 400
        else:
            try:
                record = json.loads(record)
                jsonschema.validate(record, HISCORE_JSON_SCHEMA)
            except json.decoder.JSONDecodeError:
                return "Malformed JSON", 400
            except jsonschema.ValidationError:
                return "Malformed save JSON", 400

            nickname = record["Nickname"]
            track = record["Track"]

            os.makedirs(os.path.dirname(HISCORE_FILE), exist_ok=True)

            with open(HISCORE_FILE, 'w+') as f, \
                    FileLock(HISCORE_FILE, timeout=30).acquire():
                all_hiscores_str = f.read()
                all_hiscores = json.loads(all_hiscores_str or "{}")

                prev, other = partition(lambda h: (h["Nickname"] == nickname
                                                   and h["Track"] == track),
                                        all_hiscores)
                if prev and hiscore_goodness(record) < hiscore_goodness(prev):
                    msg = "New score worse than old: {new} < {old}".format(
                            old=hiscore_summary(prev),
                            new=hiscore_summary(record))
                    logging.debug(msg)
                    return msg, 200

                logging.debug("Updating score: %s -> %s",
                              hiscore_summary(prev), hiscore_summary(record))
                all_hiscores = [record] + other
                all_hiscores.append(record)

                all_hiscores_str = json.dumps(all_hiscores)
                if len(all_hiscores_str) > HISCORE_SIZE_LIMIT:
                    # TODO: group by track
                    all_hiscores.sort(hiscore_goodness, reverse=True)
                    all_hiscores.pop()
                    all_hiscores_str = json.dumps(all_hiscores)

                f.seek(0)
                f.write(all_hiscores_str)

    return "", 200
