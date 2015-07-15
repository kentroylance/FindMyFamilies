var gulp = require('gulp');
var glue = require('gulp-sprite-glue');
//var glue = require('../');


gulp.task('sprites', function() {
  return gulp.src("./Web.UI/Images/index/*")
    .pipe(glue({
        css: '/Web.UI/Images/sprites',
        img: '/Web.UI/Images/sprites'
    }));
})

gulp.task('default', ['sprites']);

