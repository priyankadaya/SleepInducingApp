let playBtn = $('#play');
let pauseBtn = $('#pause');
const sounds = document.querySelectorAll('audio');
let soundsArr = Array.from(sounds);

$(".song-btn").click(function() {
    var key = this.getAttribute('data-sound');
    for(var i = 0; i < soundsArr.length; i++) {
        if(soundsArr[i].duration > 0) {
            soundsArr[i].pause();
        }
        if(soundsArr[i]['id'] == key) {
            soundsArr[i].play();
            currTrack = soundsArr[i];
            window.globalSound = currTrack;
        }
    }
 })

pauseBtn.click(function(){
    globalSound.pause();
})

playBtn.click(function(){
    globalSound.play();
})

