function handleReact(container, path, commentId, isLike) {

    var likeButton = container.children[0];
    var disLikeButton = container.children[1];

    var likeIcon = likeButton.querySelector('i');
    var disLikeIcon = disLikeButton.querySelector('i');

    var isLiked = () => likeIcon.classList.contains('bi-hand-thumbs-up-fill');
    var isDisliked = () => disLikeIcon.classList.contains('bi-hand-thumbs-down-fill');

    var currentIsLike = null;
    if (isLiked()) currentIsLike = true;
    if (isDisliked()) currentIsLike = false;


    function updateCount(button, isIncrease) {
        var count = button.querySelector('span');
        isIncrease
            ? (count.innerText = parseInt(count.innerText) + 1)
            : (count.innerText = parseInt(count.innerText) - 1);
    }

    fetch(`${path}?commentId=${commentId}&isLike=${isLike}`, {
        method: 'POST',
    })
        .then(res => {
            if (res.status == 401) throw new Error('Bạn chưa đăng nhập.')
            if (res.status == 404) throw new Error('Không tìm thấy bình luận.')
            if (500 <= res.status && res.status < 600) throw new Error('Lỗi hệ thống.')
        })
        .then(res => {
            if (currentIsLike == null) {
                if (isLike) {
                    likeIcon.classList.replace('bi-hand-thumbs-up', 'bi-hand-thumbs-up-fill')
                    updateCount(likeButton, true)
                }
                else {
                    disLikeIcon.classList.replace('bi-hand-thumbs-down', 'bi-hand-thumbs-down-fill')
                    updateCount(disLikeButton, true)
                }
            }
            else {
                if (currentIsLike == isLike) {
                    if (currentIsLike) {
                        likeIcon.classList.replace('bi-hand-thumbs-up-fill', 'bi-hand-thumbs-up')
                        updateCount(likeButton, false)
                    }
                    else {
                        disLikeIcon.classList.replace('bi-hand-thumbs-down-fill', 'bi-hand-thumbs-down')
                        updateCount(disLikeButton, false)
                    }
                }
                else {
                    if (isLike == true) {
                        likeIcon.classList.replace('bi-hand-thumbs-up', 'bi-hand-thumbs-up-fill')
                        disLikeIcon.classList.replace('bi-hand-thumbs-down-fill', 'bi-hand-thumbs-down')
                        updateCount(likeButton, true)
                        updateCount(disLikeButton, false)
                    }
                    else {
                        likeIcon.classList.replace('bi-hand-thumbs-up-fill', 'bi-hand-thumbs-up')
                        disLikeIcon.classList.replace('bi-hand-thumbs-down', 'bi-hand-thumbs-down-fill')
                        updateCount(likeButton, false)
                        updateCount(disLikeButton, true)
                    }
                }
            }
        })
        .catch(err => {
            const wrapper = document.createElement('div');
            wrapper.className = 'position-fixed bottom-0 end-0 p-3';
            wrapper.style.zIndex = '1050';

            wrapper.innerHTML = `
                <div class="alert alert-danger alert-dismissible fade show shadow-sm small mb-0" role="alert">
                    ${err.message}
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
            `;
            document.body.appendChild(wrapper);
        });
}