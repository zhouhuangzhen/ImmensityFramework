const alarm = {
  alarmFiles: new Map(),
  audioContext: new AudioContext(),
  audioSource: null,
  current: null,
  createSource: function() {
    var source = this.audioContext.createBufferSource()
    source.loop = true
    source.connect(this.audioContext.destination)
    return source
  },
  fetchAudio: function(e) {
    const sound = fetch(e)
      .then((resp) => resp.arrayBuffer())
      .then((buffer) => this.audioContext.decodeAudioData(buffer))
    return sound
      .then((data) => {
        this.alarmFiles.set(e, data)
        return true
      })
  },
  loadAlarmFiles: function(plocations) {
    const files = [
      ...new Set(
        plocations.features
          .filter((e) => e.properties.PAlarmFile)
          .map((e) => e.properties.PAlarmFile)
      )
    ].filter((e) => !this.alarmFiles.has(e))
    return Promise.all(files.map((e) => this.fetchAudio(e)))
  },
  updateState: function(playStatus) {
    if (this.playStatus === 'paused') {
      if (this.audioContext.state === 'running') {
        this.audioContext.suspend()
      }
    } else if (this.playStatus === 'playing') {
      if (this.audioContext.state === 'suspended') {
        this.audioContext.resume()
      }
    } else if (this.playStatus === 'stopped') {
      stop()
    }
  },
  stop: function() {
    if (this.audioSource) {
      this.audioSource.stop(0)
    }
    this.current = null
  },
  play: function(alarm) {
    if (!alarm || !this.alarmFiles.has(alarm)) {
      stop()
      return
    }
    if (this.current != alarm) {
      this.audioSource = this.createSource()
      this.current = alarm
      this.audioSource.buffer = this.alarmFiles.get(alarm)
      this.audioSource.start(0)
    }
  }
}
export default alarm
