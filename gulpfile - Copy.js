var gulp = require('gulp'),
  concat = require('gulp-concat'),
  minifycss = require('gulp-minify-css'),
  rename = require('gulp-rename'),
  notify = require('gulp-notify'),
  uglify = require('gulp-uglify');

gulp.task('css', function() {

  return gulp.src('css/bootstrap.css')
    .pipe(rename('mycontrols.min.css'))
    .pipe(minifycss())
    .pipe(gulp.dest('css'))
    .pipe(notify({message: 'Styles minified'}));

});

gulp.task('js', function() {

  return gulp.src(['scripts/jquery-1.10.2.js','scripts/respond.js','scripts/bootstrap.js'])
    .pipe(concat('mycontrols.js'))
    .pipe(gulp.dest('scripts'))
    .pipe(rename({suffix: '.min'}))
    .pipe(uglify())
    .pipe(gulp.dest('scripts'))
    .pipe(notify({message: 'Scripts merged and minified'}));

});

gulp.task('default', ['js','css'], function() {});