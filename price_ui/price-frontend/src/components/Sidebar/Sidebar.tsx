import React from 'react';
import '../../css/sidebar.css';
import { SidebarData } from './SidebarData';
import {Link} from 'react-router-dom';
import SignInButton from '../UserAuthentication/SignInButton';

function Sidebar() {
  return (
    <div className='Sidebar'>
      <div className='SidebarIcon'>
        <li className='SidebarMenuToggle'>
          X
        </li>
      </div>
      <nav className='SidebarMenu'>
          <ul className='SidebarMenuItems'>
              {SidebarData.map((item, index) => {
                return (
                  <li key={index} className={item.className}>
                    <Link to={item.path}>{item.title}</Link>
                  </li>
                )})}
              <SignInButton/>
          </ul>
      </nav>
    </div>
  );
}

export default Sidebar;