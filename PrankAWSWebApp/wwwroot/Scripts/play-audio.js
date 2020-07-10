///Paly Audio
var audioElement = document.createElement('audio');
function playAudio(id,url) {
    audioElement.setAttribute('src', url);
    $("#playaudio_" + id).hide();
    $("#pauseaudio_" + id).show();
    audioElement.play();
    audioElement.addEventListener("ended", function () {
        $("#playaudio_" + id).show();
        $("#pauseaudio_" + id).hide();
    }, true);
}

function pauseAudio(id) {   
    audioElement.pause();
    $("#playaudio_" + id).show();
    $("#pauseaudio_" + id).hide();
}
