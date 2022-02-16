import React from 'react';
import '../css/BasketNavbar.css'
import { BasketNavbarData } from './BasketNavbarData';
import BasketSearchbar from './BasketSearchbar';

function BasketNavbar() {
  return (
    <div className='BasketNavbarContainer'>
      <nav className='BasketNavbar'>
        <ul className='BasketNavbarItems'>
          {BasketNavbarData.map((item, index) => {
            return (
              <li key={index} className={item.className}>
                {item.title}
              </li>
            )
          })}
          <li className='ProductSearch'>
            <BasketSearchbar />
          </li>
        </ul>
      </nav>
    </div>
    );
}

export default BasketNavbar;
