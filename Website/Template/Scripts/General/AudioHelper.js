var WebCamPictureAudio = 'AudioLibraryCameraSound';

function PlayAudio(AudioFile) {
    if (typeof AudioFile != undefined && AudioFile != null && AudioFile.trim() != '') {
        
        if ($("#" + AudioFile).length > 0) {
            var audio = $("#" + AudioFile);
            audio[0].pause();
            audio[0].load();//suspends and restores all audio element
            audio[0].oncanplaythrough = audio[0].play();
        }
    }
}



