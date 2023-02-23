import React from 'react';
import '../../css/sidebar.css';
import { SidebarData } from './SidebarData';
import {Link} from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { authenticateUser } from '../../slices/AuthSlice';
import auth from '../UserAuthentication/CognitoAuthentication';

function Sidebar() {

  const dispatch = useDispatch()
  
  const handleSignIn = () => {
    dispatch(authenticateUser());
  };

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
              <button onClick={handleSignIn}>Sign In</button>
          </ul>
      </nav>
    </div>
  );
}

export default Sidebar;