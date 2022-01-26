import React from 'react';
import '../css/sidebar.css';
import { SidebarData } from './sidebarData';
import {Link} from 'react-router-dom';

function Sidebar() {
    return (
        <div className='sidebar'>
            <div className='sidebar-icon'>
                <li className='sidebar-menu-toggle'>
                    X
                </li>
            </div>
            <nav className='sidebar-menu'>
                <ul className='sidebar-menu-items'>
                        {SidebarData.map((item, index) => {
                                return (
                                        <li key={index} className={item.className}>
                                            <Link to={item.path}>{item.title}</Link>
                                        </li>
                                    )})}
                </ul>
            </nav>
        </div>
    );
}

export default Sidebar;