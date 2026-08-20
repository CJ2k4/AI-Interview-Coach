window.audioRecorder = {
    mediaRecorder: null,
    audioChunks: [],
    
    startRecording: async function () {
        try {
            const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
            this.mediaRecorder = new MediaRecorder(stream);
            this.audioChunks = [];

            this.mediaRecorder.ondataavailable = event => {
                if (event.data && event.data.size > 0) {
                    this.audioChunks.push(event.data);
                }
            };

            this.mediaRecorder.start();
            return true;
        } catch (error) {
            console.error("Error accessing microphone:", error);
            return false;
        }
    },

    stopRecording: function () {
        return new Promise((resolve) => {
            if (!this.mediaRecorder) {
                resolve(null);
                return;
            }

            this.mediaRecorder.onstop = () => {
                // Let browser choose default MIME type
                const audioBlob = new Blob(this.audioChunks);
                
                // Stop all tracks to release microphone
                this.mediaRecorder.stream.getTracks().forEach(track => track.stop());
                
                // Read blob as array buffer to pass back to Blazor
                const reader = new FileReader();
                reader.onloadend = () => {
                    const arrayBuffer = reader.result;
                    const uint8Array = new Uint8Array(arrayBuffer);
                    resolve(uint8Array);
                };
                reader.readAsArrayBuffer(audioBlob);
            };

            this.mediaRecorder.stop();
        });
    }
};
