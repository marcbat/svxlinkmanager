#!/bin/bash

# Stop svxlink
if pgrep -x svxlink >/dev/null
then
    pkill -TERM svxlink
fi