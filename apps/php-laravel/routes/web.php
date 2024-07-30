<?php

use Illuminate\Support\Facades\Route;

Route::get('/', function () {
    return view('welcome');
});

Route::get('/test', function() {
    $randomNumber = rand(1, 1000);
    Log::channel('logstash')->info("Rehagro Laravel Log Test $randomNumber");
    return ['Hello', 'World'];
  });
