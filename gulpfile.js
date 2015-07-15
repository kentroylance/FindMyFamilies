var gulp = require('gulp');
var spritesmith = require('gulp.spritesmith');

gulp.task('copyIndex', function() {
    return gulp.src('Web.UI/Images/layout/**/*')
        .pipe(gulp.dest('Web.UI/Images/index/'))
});

gulp.task('index', ['copyIndex'], function () {
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

gulp.task('copyResearch', function() {
    return gulp.src('Web.UI/Images/layout/**/*')
        .pipe(gulp.dest('Web.UI/Images/research/'))
});

gulp.task('research', ['copyResearch'], function () {
  var spriteData = gulp.src('Web.UI/Images/research/*').pipe(spritesmith({
    imgName: 'research.png',
    cssName: 'research.css',
	cssOpts: {
	  cssSelector: function (item) {
		return '.fmf-' + item.name;
	  }
	}
  }));
  return spriteData.pipe(gulp.dest('Web.UI/Images/sprites/'));
});

gulp.task('default', ['index', 'research']);


