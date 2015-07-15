define(function(require) {
    
    var $ = require('jquery');
    require('fancybox');

    $('.fancyboxvideo').fancybox({
        openEffect: 'none',
        closeEffect: 'none',
        helpers: {
            media: true,
            title: {
                type: 'inside'
            }
        },
        fitToView: false,
        aspectRatio: true,
        maxWidth: "100%",
        maxHeight: "100%",
        beforeLoad: function () {
            this.title = $(this.element).attr('caption');
        },
        afterLoad: function () {
            this.width = $(this.element).data("width");
            this.height = $(this.element).data("height");
        }
    });

    $(".fancybox").fancybox({
        padding: 5,
        width: 1050,
        height: 800,
        openEffect: 'none',
        closeEffect: 'none',
        iframe: {
            preload: false
        }
    });

    var fancybox = {
    };

    return fancybox;
});

