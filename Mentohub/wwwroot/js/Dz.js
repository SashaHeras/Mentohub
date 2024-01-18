class Dz {
    constructor() {

    }

    initDropzone(base64Video, videoLength) {
        // Configure Dropzone
        Dropzone.options.myDropzone = {
            paramName: "file", // The name that will be used to transfer the file
            maxFilesize: 109457697, // MB
            addRemoveLinks: true,
            // Add any other Dropzone configuration options you need
        };

        // Load existing video file into Dropzone
        var existingFile = {
            name: "existing-video.mp4", // Name of the video file
            size: videoLength, // Size of the file in bytes
            dataURL: "data:video/mp4;base64," + base64Video, // Base64 representation of the video file
            // Add any other relevant information
        };

        var blob = dataURItoBlob(existingFile.dataURL);
        var mockFile = new File([blob], existingFile.name, { type: "video/mp4", size: existingFile.size });

        var myDropzone = new Dropzone("#myDropzone");
        myDropzone.addFile(mockFile);

        // Helper function to convert data URI to Blob
        function dataURItoBlob(dataURI) {
            var byteString = atob(dataURI.split(',')[1]);
            var ab = new ArrayBuffer(byteString.length);
            var ia = new Uint8Array(ab);
            for (var i = 0; i < byteString.length; i++) {
                ia[i] = byteString.charCodeAt(i);
            }
            return new Blob([ab]);
        }
    }
}