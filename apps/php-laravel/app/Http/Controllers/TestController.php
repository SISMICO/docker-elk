<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\View\View;

class TestController extends Controller
{
    public function show() : View {
        return ['Hello', 'World'];
    }
}
