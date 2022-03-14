import '../css/Item.css';
import SingleItemList from '../components/SingleItem/SingleItemList';
import SingleItemNavbar from '../components/SingleItem/SingleItemNavbar';
import SingleItemInfo from '../components/SingleItem/SingleItemInfo';

export default function Item() {
  return (
    <div className='ItemContainer'>
      <div className='SingleItemInfoContainer'>
        <SingleItemInfo/>
      </div>
      <div className='SingleItemNavbarContainer'>
        <SingleItemNavbar/>
      </div>
      <div className='SingleItemListContainer'>
        <SingleItemList/>
      </div>
    </div>
  )
}
