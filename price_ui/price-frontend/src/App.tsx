import React from 'react';
import './css/main.css';
import Sidebar from './components/Sidebar';
import {BrowserRouter, Routes, Route} from "react-router-dom";
import ItemBasket from './pages/ItemBasket';
import Home from './pages/Home';

function App() {
  return (
    <div className="App">
      <div className='WebTitle'>
        <header>
          <h1 className='TitleText'>Price Tracker</h1>
        </header>
      </div>
      <BrowserRouter>
        <Sidebar />
        <Routes>
          <Route path="/" element={<Home/>}/>
          <Route path="/ItemBasket" element={<ItemBasket/>}/>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
