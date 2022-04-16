/**
 * @jest-environment jsdom
 */

import React, {useState} from 'react';
import nock from 'nock';
import { render, screen, shallow, fireEvent, mount } from '@testing-library/react';
import 'regenerator-runtime/runtime';
import '@testing-library/jest-dom';
import HomePage from './pages/HomePage.js';
import LoginPage from './pages/LoginPage.js';
import AccountPage from './pages/AccountPage.js';

test('renders login Text in HomePage', () => {
  render(<HomePage />);
  const linkElement = screen.getByText(/login/i);
  expect(linkElement).toBeInTheDocument();
});

test('renders Create Account Text in LoginPage', () => {
  render(<LoginPage />);
  const linkElement = screen.getByText(/Create Account/i);
  expect(linkElement).toBeInTheDocument();
});


// other attempts, give errors
/*it('displays user data', async () => {
  const scope = nock('https://yoursite.com')
    .get('/api')
    .once()
    .reply(200, {
      data: 'response',
    });

  var {getByTestId, findByTestId} = render(<LoginPage />)
  fireEvent.click(find("#loginName"))
  expect(await find("#loginName")).toHaveTextContent('response');
})*/

/*test('accnt Page', async () => {
  const w = shallow(<AccountPage />);
  const changePass = w.container.querySelector('#accountchangePassBtn');

  changePass.simulate('click');
  const countState = wrapper.state().page;
  //expect(countState).toEqual(2);

  expect(screen.getByText(/Here we will change password/i)).toBeInTheDocument();
})*/