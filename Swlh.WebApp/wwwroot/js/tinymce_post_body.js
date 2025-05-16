tinymce.addI18n('minimal', {
    'General': 'Audio',
    'Embed': 'Youtube',
    'Paste your embed code below:': 'Paste Youtube embed code below:',
});

tinymce.init({
    selector: '#editor',
    language: 'minimal',
    license_key: 'gpl',
    statusbar: false,
    valid_elements: '*[*]',


    paste_as_text: true,

    extended_valid_elements: 'iframe[src|srcdoc|style|width|height|scrolling|frameborder|allowfullscreen]',
    valid_children: '+body[iframe]',

    plugins: 'image media link fullscreen table lists code',
    //toolbar: 'paragraph | strikethrough | bullist numlist outdent indent | media link image table fullscreen | heading',
    toolbar: 'undo redo | styles | bold italic underline strikethrough forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image media table codesample | blockquote hr removeformat | fullscreen preview code',
    file_picker_types: 'image media file',
    height: 600,
    content_css: 'default,/css/tinymce_custom.css',


    media_alt_source: false,
    media_poster: false,


    link_default_target: '_blank',
    default_link_target: '_blank',
    relative_urls: true,         
    remove_script_host: true,
    convert_urls: false,

    video_template_callback: data => `
    <span>
        <audio><source src="${data.source}"></audio>
        <button class="play-button" onclick="this.parentElement.querySelector('audio').play()"></button>
    </span>`,

    file_picker_callback: function (callback, value, meta) {
        const input = document.createElement('input');
        input.type = 'file';
        if (meta.filetype === 'image') input.accept = 'image/*';
        if (meta.fieldname === 'audioUrl') input.accept = 'audio/*';
        input.onchange = function () {
            const file = this.files[0];
            const formData = new FormData();
            formData.append('file', file);
            fetch('/CustomFiles/Create', {
                method: 'POST',
                body: formData,
                headers: {
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                }
            })
                .then(res => res.json())
                .then(data => callback(data.location))
                .catch(err => {
                    console.error('Upload failed:', err);
                    alert('Upload thất bại.');
                });
        };
        input.click();
    }
});