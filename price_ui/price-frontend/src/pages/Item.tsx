import '../css/Item.css';
import SingleItemList from '../components/SingleItem/SingleItemList';
import SingleItemNavbar from '../components/SingleItem/SingleItemNavbar';

export default function Item() {
  return (
    <div className='ItemContainer'>
      <div className='SingleItemNavbarContainer'>
        <SingleItemNavbar/>
      </div>
      <div className='SingleItemListContainer'>
        <SingleItemList/>
      </div>
    </div>
  )
}
