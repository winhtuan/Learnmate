window.voiceAssistant = {
    startListening: function (dotnetHelper, callbackMethod) {
        if (!('webkitSpeechRecognition' in window) && !('SpeechRecognition' in window)) {
            alert('Trình duyệt của bạn không hỗ trợ tính năng nhận diện giọng nói (Web Speech API). Vui lòng dùng Chrome hoặc Edge mới nhất.');
            return;
        }

        const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;
        const recognition = new SpeechRecognition();
        
        recognition.lang = 'vi-VN';
        recognition.interimResults = false;
        recognition.maxAlternatives = 1;

        recognition.onresult = function (event) {
            const transcript = event.results[0][0].transcript;
            dotnetHelper.invokeMethodAsync(callbackMethod, transcript);
        };

        recognition.onerror = function (event) {
            console.error("Lỗi nhận diện giọng nói: ", event.error);
            let detail = event.error;
            if (event.error === 'network') {
                detail = 'network: Không thể kết nối tới máy chủ nhận diện giọng nói. Kiểm tra kết nối Internet và thử lại.';
            } else if (event.error === 'not-allowed') {
                detail = 'not-allowed: Trình duyệt không có quyền truy cập microphone. Vui lòng cấp quyền và thử lại.';
            } else if (event.error === 'no-speech') {
                detail = 'no-speech: Không nhận được giọng nói. Vui lòng thử lại.';
            }
            dotnetHelper.invokeMethodAsync(callbackMethod, "ERROR: " + detail);
        };

        recognition.start();
    }
};
