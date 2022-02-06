import React from 'react';
import '../css/BasketNavbar.css'
import { BasketNavbarData } from './BasketNavbarData';

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
                <li className='SearchItem'>
                    Search
                </li>
            </ul>
        </nav>
    </div>
    );
}

export default BasketNavbar;
