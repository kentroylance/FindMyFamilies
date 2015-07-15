var gulp = require('gulp');
var msbuild = require('gulp-msbuild');
var del = require('del');
var uglify = require('gulp-uglify');

var config = {
   //Include all js files but exclude any min.js files
    src : ['Scripts/view/*.js', '!app/**/*.min.js'],
}

// Synchronously delete the output file(s)
gulp.task('clean', function(){
  del.sync(['../../Production/Web/Scripts/view/*.*'], {force: true})
});



// Combine and minify all files from the app folder
gulp.task('scripts', ['clean'], function() {
  return gulp.src(config.src)
    .pipe(uglify())
    .pipe(gulp.dest('../../Production/Web/Scripts/view/'), {force: true});
});

gulp.task('build', function() {
    return gulp
        .src('../**/*.sln')
        .pipe(msbuild({
            toolsVersion: 12.0,
            targets: ['Clean', 'Build'],
            errorOnFail: true,
            stdout: true
        }));
});

gulp.task('default', ['scripts']);