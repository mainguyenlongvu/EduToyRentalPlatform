document.getElementById('messageForm').addEventListener('submit', async function (event) {
    event.preventDefault(); // Ngăn chặn hành động gửi mặc định của biểu mẫu

    const messageInput = document.getElementById('messageInput');
    const messageText = messageInput.value;

    if (messageText.trim() === '') {
        return; // Không thêm tin nhắn rỗng
    }

    // Thêm tin nhắn mới vào giao diện
    const messageContainer = document.getElementById('messageContainer');
    const messageItem = document.createElement('div');
    messageItem.className = 'message-item';

    const messageBubble = document.createElement('div');
    messageBubble.className = 'message-bubble';
    messageBubble.textContent = messageText;

    const messageInfo = document.createElement('div');
    messageInfo.className = 'message-info';
    const now = new Date();
    messageInfo.textContent = now.getHours() + ':' + (now.getMinutes() < 10 ? '0' : '') + now.getMinutes();

    messageItem.appendChild(messageBubble);
    messageItem.appendChild(messageInfo);
    messageContainer.appendChild(messageItem);

    // Gửi tin nhắn đến máy chủ
    try {
        const response = await fetch('/Message?handler=SendMessage', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ messageInput: messageText })
        });

        if (!response.ok) {
            throw new Error('Có lỗi xảy ra khi gửi tin nhắn.');
        }
    } catch (error) {
        console.error('Error:', error);
        // Bạn có thể thêm logic để hiển thị thông báo lỗi cho người dùng nếu cần
    }

    messageInput.value = ''; // Xóa nội dung ô nhập sau khi gửi
    messageContainer.scrollTop = messageContainer.scrollHeight; // Cuộn xuống dưới cùng
});
