<script>
import Hls from 'hls.js';
export default {
  props: {
    src: {
      type: String,
      required: true
    },
    sasToken: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      hls: null,
      levels: [],
      selectedLevel: -1
    };
  },
  mounted() {
    this.initializePlayer();
  },
  watch: {
    src(newSrc, oldSrc) {
      console.log(`HLS source updated from ${oldSrc} to ${newSrc}`);
      this.initializePlayer();
    },
    selectedLevel(newLevel) {
      if (this.hls) {
        this.hls.currentLevel = newLevel;
      }
    }
  },
  methods: {
    initializePlayer() {
      if (!this.src) {
        console.log("No source provided for HLS player.");
        return;
      }

      const video = this.$refs.video;

      if (Hls.isSupported()) {
        if (this.hls) {
          this.hls.destroy();
        }

        this.hls = new Hls({
          xhrSetup: (xhr, url) => {
            xhr.setRequestHeader('Authorization', this.sasToken);
            xhr.open('GET', `${url}?${this.sasToken}`, true);
          }
        });

        this.hls.loadSource(this.src);
        this.hls.attachMedia(video);
        this.hls.on(Hls.Events.MANIFEST_PARSED, () => {
          this.levels = this.hls.levels;
          video.play();
        });

        video.addEventListener('ended', this.handleVideoEnd);

      } else if (video.canPlayType('application/vnd.apple.mpegurl')) {
        video.src = this.src;
        video.addEventListener('loadedmetadata', () => {
          video.play();
        });
        video.addEventListener('ended', this.handleVideoEnd);
      }
    },
    handleVideoEnd() {
      this.$emit('ended');
    },
    selectLevel(event) {
      this.selectedLevel = event.target.value;
    }
  },
  beforeDestroy() {
    if (this.hls) {
      this.hls.destroy();
    }
  }
};
</script>

<template>
    <video ref="video" width="60%" height="60%" controls></video>
    <div>
      <label for="quality">Quality:</label>
      <select id="quality" @change="selectLevel">
        <option value="-1">Auto</option>
        <option v-for="(level, index) in levels" :key="index" :value="index">
          {{ level.height }}p
        </option>
      </select>
    </div>
</template>
