#!/bin/bash

# Quelle: Eingabevideo
input="/Users/niklioni/Downloads/Lektion4.mov"

# Ausgabe-Ordner
output="/Users/niklioni/Desktop/Anikatze/ITL/TestVideoFirst"

# Auflösungen und Bitraten
bitrates=("300000" "800000" "1400000" "3000000")
resolutions=("426x240" "640x360" "842x480" "1280x720")

mkdir -p $output

for i in ${!bitrates[@]}; do
  bitrate=${bitrates[$i]}
  resolution=${resolutions[$i]}
  bufsize=$((${bitrate} * 2))

  ffmpeg -i $input -vf "scale=${resolution}" -c:a aac -ar 48000 -c:v h264 -profile:v main -crf 20 -sc_threshold 0 -g 48 -keyint_min 48 -hls_time 4 -hls_playlist_type vod -b:v ${bitrate} -maxrate ${bitrate} -bufsize ${bufsize} -hls_segment_filename "${output}/${bitrate}_%03d.ts" ${output}/${bitrate}.m3u8
done

# Master-Playlist erstellen
echo "#EXTM3U" > $output/master.m3u8
for i in ${!bitrates[@]}; do
  bitrate=${bitrates[$i]}
  resolution=${resolutions[$i]}
  echo "#EXT-X-STREAM-INF:BANDWIDTH=${bitrate},RESOLUTION=${resolution}" >> $output/master.m3u8
  echo "${bitrate}.m3u8" >> $output.master.m3u8
done
