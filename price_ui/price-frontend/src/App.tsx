import React from 'react';
import './css/main.css';
import Sidebar from './components/Sidebar';
import {BrowserRouter, Routes, Route} from "react-router-dom";
import ItemBasket from './pages/ItemBasket';
import Home from './pages/Home';
import Product from './pages/Product';

function App() {
  return (
    <div className="App">
      <div className='WebTitle'>
        <header>
          <h1 className='TitleText'>Spendy</h1>
        </header>
      </div>
      <BrowserRouter>
        <Sidebar />
        <Routes>
          <Route path="/" element={<Home/>}/>
          <Route path="/ItemBasket" element={<ItemBasket/>}/>
          <Route path="/Product" element={<Product/>}/>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
