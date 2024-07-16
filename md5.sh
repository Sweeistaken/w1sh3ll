#!/usr/bin/bash
find -type f -exec md5sum "{}" + > project.md5
