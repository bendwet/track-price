import React from 'react';
import './css/main.css';
import Sidebar from './components/sidebar';
import {BrowserRouter, Routes, Link, Route} from "react-router-dom";
import FoodBasket from './pages/FoodBasket';
import Home from './pages/Home';

function App() {
  return (
    <div className="App">
      <div className='web-title'>
        <h1 className='title-text'>Price Tracker</h1>
      </div>
      <BrowserRouter>
        <Sidebar />
        <Routes>
          <Route path="/" element={<Home/>}/>
          <Route path="/FoodBasket" element={<FoodBasket/>}/>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
