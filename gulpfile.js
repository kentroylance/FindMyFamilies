var gulp = require('gulp');
var spritesmith = require('gulp.spritesmith');

gulp.task('copyIndex', function() {
    return gulp.src('Web.UI/Content/images/layout/**/*')
        .pipe(gulp.dest('Web.UI/Content/images/index/'))
});

gulp.task('index', ['copyIndex'], function () {
  var spriteData = gulp.src('Web.UI/Content/images/index/*').pipe(spritesmith({
    imgName: 'index.png',
    cssName: 'index-sprite.css',
	cssOpts: {
	  cssSelector: function (item) {
		return '.fmf-' + item.name;
	  }
	}
  }));
  return spriteData.pipe(gulp.dest('Web.UI/Content/css/'));
});

gulp.task('copyResearch', function() {
    return gulp.src('Web.UI/Content/images/layout/**/*')
        .pipe(gulp.dest('Web.UI/Content/images/research/'))
});

gulp.task('research', ['copyResearch'], function () {
  var spriteData = gulp.src('Web.UI/Content/images/research/*').pipe(spritesmith({
    imgName: 'research.png',
    cssName: 'research-sprite.css',
	cssOpts: {
	  cssSelector: function (item) {
		return '.fmf-' + item.name;
	  }
	}
  }));
  return spriteData.pipe(gulp.dest('Web.UI/Content/css/'));
});

gulp.task('default', ['index', 'research']);


