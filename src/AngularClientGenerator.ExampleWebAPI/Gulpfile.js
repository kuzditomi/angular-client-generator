var gulp = require('gulp');
var browserify = require('browserify');
var tsify = require('tsify');
var gutil = require('gulp-util');
var source = require('vinyl-source-stream');
var rename = require('gulp-rename');

gulp.task('generated', function () {
    return browserify({
        basedir: '.',
        debug: true,
        entries: "app/generated.ts"
    })
        .plugin(tsify)
        .bundle()
        .pipe(source('bundle.js'))
        .pipe(rename('bundle-generated.js'))
        .pipe(gulp.dest('app/build'))
        .on('error', gutil.log);
});

gulp.task('app', function () {
    return browserify({
        basedir: '.',
        debug: true,
        entries: "app/example-module.ts"
    })
        .plugin(tsify)
        .bundle()
        .pipe(source('bundle.js'))
        .pipe(gulp.dest('app/build'))
        .on('error', gutil.log);
});

gulp.task('default', ['app', 'generated']);