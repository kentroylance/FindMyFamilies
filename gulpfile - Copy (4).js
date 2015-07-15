var gulp = require('gulp');
var spritesmith = require('gulp.spritesmith');

gulp.task('sprite', function () {
  var spriteData = gulp.src('Web.UI/Images/index/*').pipe(spritesmith({
    imgName: 'index.png',
    cssName: 'index.css',
	cssOpts: {
	  cssSelector: function (item) {
		return '.fmf-' + item.name;
	  }
	}
  }));
  return spriteData.pipe(gulp.dest('Web.UI/Images/sprites/'));
});

gulp.task('copy', function() {
    gulp.src('Web.UI/Images/layout/**/*')
        .pipe(gulp.dest('Web.UI/Images/index/'))
});

gulp.task('default', ['copy', 'sprite']);

