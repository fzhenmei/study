class StoreController < ApplicationController
  def index
    increment_count
    @products = Product.order(:title)
  end
end
