import React from 'react';
import '../../css/sidebar.css';
import { SidebarData } from './SidebarData';
import {Link} from 'react-router-dom';

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
              <a href='https://spendy.auth.ap-southeast-2.amazoncognito.com/login?client_id=44gdcdjjl2980ph5ismn3sr86m&response_type=token&scope=aws.cognito.signin.user.admin+email+openid+phone+profile&redirect_uri=http://localhost:3000'>
                <button>Sign In</button>
              </a>
          </ul>
      </nav>
    </div>
  );
}

export default Sidebar;