import React from "react";
import BasketNavbar from "../components/BasketNavbar";
import ItemList from "../components/ItemList";

function ItemBasket() {
    return (
        <div className="ItemBasket">
            <h1 className="ItemBasketText">ItemBasket</h1>
            <div>
                <BasketNavbar />
            </div>
            <div>
                <ItemList />
            </div>
        </div>
    );
}

export default ItemBasket;