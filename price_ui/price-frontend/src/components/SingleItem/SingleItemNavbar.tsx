import React from 'react'
import {SingleItemNavbarData} from './SingleItemNavbarData'

export default function SingleItemNavbar() {
  return (
    <div>
			<nav className='SingleItemNavbar'>
				<ul className='SingleItemNavbarItems'>
					{SingleItemNavbarData.map((item, index) => {
						return (
							<li key={index} className={item.className}>
								{item.title}
							</li>
						)
					})}
				</ul>
			</nav>
    </div>
  );
}
