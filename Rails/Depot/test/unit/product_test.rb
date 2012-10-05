require 'test_helper'

class ProductTest < ActiveSupport::TestCase

  def new_product(image_url)
    Product.new(title: "my book",
                description: "my test book",
                image_url:image_url,
                price: 10.01)
  end
  # test "the truth" do
  #   assert true
  # end

  test "product attributes must not empty" do
    product = Product.new
    assert product.invalid?
    assert product.errors[:title].any?
    assert product.errors[:description].any?
    assert product.errors[:image_url].any?
    assert product.errors[:price].any?
  end

  test "price must be positive" do
    product = Product.new(title: "my book", description: "my test book", image_url:"abc.jpg")
    product.price = -1
    assert product.invalid?
    product.errors[:price].any?

    product.price = 0
    assert product.invalid?
    product.errors[:price].any?

    product.price = 1
    assert product.valid?
  end

  test "valid image extension" do
    good = %w{fred.gif fred.jpg fred.png FRED.JPG FRED.Jpg http://a.b.c/x/y/z/fred.gif}
    bad = %w{fred.doc fred.gif/more fred.gif.more}

    good.each { |g| assert new_product(g).valid? }
    bad.each { |g| assert new_product(g).invalid? }
  end

  test "product is not valid without a unique title" do
    product = Product.new(title:       products(:ruby).title,
                          description: "yyy",
                          price:       1,
                          image_url:   "fred.gif")

    assert product.invalid?
    assert_equal ["has already been taken"], product.errors[:title]
  end

  test "product is not valid without a unique title - i18n" do
    product = Product.new(title:       products(:ruby).title,
                          description: "yyy",
                          price:       1,
                          image_url:   "fred.gif")

    assert product.invalid?
    assert_equal [I18n.translate('activerecord.errors.messages.taken')],
                 product.errors[:title]
  end
end
