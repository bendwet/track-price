import React from 'react';
import './css/main.css';
import Sidebar from './components/Sidebar/Sidebar';
import {BrowserRouter, Routes, Route} from "react-router-dom";
import ItemBasket from './pages/ItemBasket';
import Home from './pages/Home';
import Item from './pages/Item';
import SignUp from './pages/SignUp';

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
          <Route path="/itembasket" element={<ItemBasket/>}/>
          <Route path="/item/:productId" element={<Item/>}/>
          <Route path="/signup" element={<SignUp/>}/>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
