import React from "react";
import '../css/ItemBasket.css'
import BasketNavbar from "../components/UserBasket/BasketNavbar";
import ItemList from "../components/UserBasket/ItemList";
import AddItem from "../components/UserBasket/AddItem";

function ItemBasket() {
    return (
      <div className="ItemBasket">
        <h1 className="ItemBasketText">ItemBasket</h1>
        <div className="TopBarContainer">
          <div className="AddItemButton">
            <AddItem />
          </div>
          <div className="BasketNavbar"> 
            <BasketNavbar />
          </div>
        </div>
        <div>
          <ItemList />
        </div>
      </div>
    );
}

export default ItemBasket;