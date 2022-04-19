/**
 * @jest-environment jsdom
 */

import React, {useState} from 'react';
import ReactDOM from 'react-dom';
import App from './App.js';
import nock from 'nock';
import { render, screen, fireEvent, mount } from '@testing-library/react';

import Enzyme from 'enzyme';
import { shallow, configure } from 'enzyme';

import Adapter from '@wojtekmaj/enzyme-adapter-react-17';

import 'regenerator-runtime/runtime';
import '@testing-library/jest-dom';
import HomePage from './pages/HomePage.js';
import RegisterPage from './pages/RegisterPage.js';
import LoginPage from './pages/LoginPage.js';
import PasswordResetPage from './pages/PasswordResetPage.js';
import EmailConfirmPage from './pages/EmailConfirmPage.js';
import GoToLoginPageBtn from './pages/HomePage.js';
import AccountPage from './pages/AccountPage.js';

Enzyme.configure({ adapter: new Adapter() });

it('renders app without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<App />, div);
  ReactDOM.unmountComponentAtNode(div);
})

test('renders login Text in HomePage', () => {
  render(<HomePage />);
  const linkElement = screen.getByText(/login/i);
  expect(linkElement).toBeInTheDocument();
});

test('renders Register Text in RegisterPage', () => {
  render(<RegisterPage />);
  const linkElement = screen.getByText(/Register/i);
  expect(linkElement).toBeInTheDocument();
});

test('renders Create Account Link in LoginPage', () => {
  render(<LoginPage />);
  const linkElement = screen.getByText(/Create Account/i);
  expect(linkElement).toBeInTheDocument();
});

test('renders Enter your account email Text in PasswordResetPage', () => {
  render(<PasswordResetPage />);
  const linkElement = screen.getByText(/Enter your account email/i);
  expect(linkElement).toBeInTheDocument();
});

test('renders Please Verify Your Account in EmailConfirmPage', () => {
  render(<EmailConfirmPage />);
  const linkElement = screen.getByText(/Please Verify Your Account/i);
  expect(linkElement).toBeInTheDocument();
});

/*
describe('Test Button component', () => {
  it('test click event', () => {
    const mockCallBack = jest.fn();

    const button = shallow((<HomePage/>));
    button.find('button').simulate('click');
    expect(mockCallBack.mock.calls.length).toEqual(1);
  });
});
*/




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