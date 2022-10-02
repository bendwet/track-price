import React from 'react'
import { RootState } from '../../stores/store';
import {useDispatch, useSelector } from 'react-redux'
import { signUp } from '../../slices/SignUpSlice';
import { setEmail, setPassword } from '../../slices/SignUpSlice';
import User from '../Models/User';
import UserPool from '../../UserPool';

function SignUpForm() {

  const dispatch = useDispatch();
  const {email, password} = useSelector((state: RootState) => state.signup)
  
  const onSubmit = (event: any) => {
    event.preventDefault();
    dispatch(signUp({email, password} as User))
  };

  return (
    <div>
      <h1>SignUpForm</h1>
      <form onSubmit={onSubmit}>
        <label>
          Email
        </label>
        <input onChange={(event) => dispatch(setEmail(event.target.value))}/>
        <label>
          Password
        </label>
        <input onChange={(event) => dispatch(setPassword(event.target.value))}/>
        <button type='submit'>Sign Up</button>
      </form>
    </div>
  );
}

export default SignUpForm;
