# gunicorn-flask

FROM ubuntu:18.04

ENV DEBIAN_FRONTEND noninteractive

RUN apt-get update
RUN apt-get install -y python3 python3-pip python3-virtualenv gunicorn3

# Setup flask application
RUN mkdir -p /deploy/app
COPY gunicorn_config.py /deploy/gunicorn_config.py
COPY app /deploy/app
RUN pip3 install -r /deploy/app/requirements.txt
WORKDIR /deploy/app

EXPOSE 5000

# Start gunicorn
CMD ["/usr/bin/gunicorn3", "--config", "/deploy/gunicorn_config.py", "a@_server:app"]
