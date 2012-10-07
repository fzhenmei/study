class ApplicationController < ActionController::Base
  protect_from_forgery

  def increment_count
    session[:count] ||= 0
    session[:count] += 1
  end

  def setcount_zero
    session[:count] = 0
  end

  private

  def current_cart
    Cart.find(session[:cart_id])
  rescue  ActiveRecord::RecordNotFound
    cart = Cart.create
    session[:cart_id] = cart.id
    cart
  end
end
