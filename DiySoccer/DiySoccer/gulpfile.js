/*!
 * gulp
 * $ npm install gulp-ruby-sass gulp-autoprefixer gulp-cssnano gulp-jshint gulp-concat gulp-uglify gulp-imagemin gulp-notify gulp-rename gulp-livereload gulp-cache del --save-dev
 */

// Load plugins
var gulp = require('gulp'),
    sass = require('gulp-ruby-sass'),
    autoprefixer = require('gulp-autoprefixer'),
    cssnano = require('gulp-cssnano'),
    jshint = require('jshint'),
    jshint = require('gulp-jshint'),
    uglify = require('gulp-uglify'),
    rename = require('gulp-rename'),
    concat = require('gulp-concat'),
    notify = require('gulp-notify'),
    cache = require('gulp-cache'),
    livereload = require('gulp-livereload'),
    del = require('del');
var Promise = require('es6-promise').Promise;

// Styles
gulp.task('styles', function () {
    return sass('content/scss/default.scss', { style: 'expanded' })
      .pipe(autoprefixer('last 2 version'))
      .pipe(gulp.dest('content/dist/css'))
      .pipe(rename({ suffix: '.min' }))
      .pipe(cssnano())
      .pipe(gulp.dest('content/dist/css'))
      .pipe(notify({ message: 'Styles task complete' }));
});

// Scripts
gulp.task('scripts', function () {
    return gulp.src('content/js/**/*.js')
      .pipe(jshint('.jshintrc'))
      .pipe(jshint.reporter('default'))
      .pipe(concat('main.js'))
      .pipe(gulp.dest('content/dist/js'))
      .pipe(rename({ suffix: '.min' }))
      .pipe(uglify())
      .pipe(gulp.dest('content/dist/js'))
      .pipe(notify({ message: 'Scripts task complete' }));
});

// Clean
gulp.task('clean', function () {
    return del(['dist/styles']);
});

// Watch
gulp.task('watch', function () {

    // Watch .scss files
    gulp.watch('content/scss/**/*.scss', ['styles']);

    // Watch .js files
    gulp.watch('content/js/**/*.js', ['scripts']);

    // Create LiveReload server
    livereload.listen();

    // Watch any files in dist/, reload on change
    gulp.watch(['content/dist/**']).on('change', livereload.changed);

});

