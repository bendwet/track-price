import React from 'react';
import { RootState } from '../../stores/store';
import { useDispatch, useSelector } from 'react-redux';
import { authenticateUser, signOutUser } from '../../slices/AuthSlice';
import auth from './CognitoAuthentication';

export default function SignInButton() {
  const dispatch = useDispatch();

  // get state of user authentication from store
  const isAuthenticated = useSelector((state: RootState) => state.auth.isAuthenticated);
  const error = useSelector((state: RootState) => state.auth.error);
  const token = useSelector((state: RootState) => state.auth.accessToken);

  const handleSignIn = () => {
    auth.useCodeGrantFlow();
    auth.getSession();
  }

  const handleSignOut = () => {
    auth.signOut();
  }

  console.log(isAuthenticated);

  if (isAuthenticated) {
    return (
      <div>
          <button onClick={handleSignOut}>Sign Out</button>
      </div>
    )
  }
  return (
    <div>
        <button onClick={handleSignIn}>Sign In</button>
    </div>
  )
}
